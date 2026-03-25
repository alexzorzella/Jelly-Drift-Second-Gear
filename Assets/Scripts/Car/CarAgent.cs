using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class CarAgent : Agent {
    Car car;
    Transform[] nodes;
    int currentNodeIndex;

    Vector3 startPosition;
    Quaternion startRotation;

    // How many waypoints ahead to include in observations
    const int lookaheadCount = 3;

    public void Initialize(Car car, Transform path) {
        this.car = car;
        nodes = path.GetComponentsInChildren<Transform>();
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public override void OnEpisodeBegin() {
        car.rb.linearVelocity = Vector3.zero;
        car.rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(startPosition, startRotation);
        currentNodeIndex = 0;
    }

    // Total observations: 1 + 1 + 3 + (3+1)*lookaheadCount = 5 + 4*3 = 17
    public override void CollectObservations(VectorSensor sensor) {
        sensor.AddObservation(car.speed / 200f);
        sensor.AddObservation(car.steerAngle / 37f);
        sensor.AddObservation(car.acceleration / 50f); // Vector3 = 3 floats

        for (int i = 0; i < lookaheadCount; i++) {
            int idx = (currentNodeIndex + i + 1) % nodes.Length;
            Vector3 toNode = transform.InverseTransformPoint(nodes[idx].position);
            sensor.AddObservation(toNode.normalized);
            sensor.AddObservation(Mathf.Clamp01(toNode.magnitude / 50f));
        }
    }

    // Action spec: ContinuousActions[2] = {steering, throttle}, DiscreteActions[1] = {brake}
    public override void OnActionReceived(ActionBuffers actions) {
        car.steering = actions.ContinuousActions[0];
        car.throttle = Mathf.Clamp01(actions.ContinuousActions[1]);
        car.braking  = actions.DiscreteActions[0] == 1;

        // Small reward for forward speed, penalty per step to encourage efficiency
        AddReward(car.speed * 0.0001f);
        AddReward(-0.001f);

        if (transform.position.y < -10f) {
            AddReward(-1f);
            EndEpisode();
        }
    }

    // Called from CheckpointUser when a checkpoint is cleared
    public void OnCheckpointReached(int nodeIndex) {
        currentNodeIndex = nodeIndex;
        AddReward(1f);
    }

    public override void Heuristic(in ActionBuffers actionsOut) {
        var c = actionsOut.ContinuousActions;
        var d = actionsOut.DiscreteActions;
        c[0] = Input.GetAxis("Horizontal");
        c[1] = Input.GetKey(KeyCode.W) ? 1f : 0f;
        d[0] = Input.GetKey(KeyCode.Space) ? 1 : 0;
    }
}
