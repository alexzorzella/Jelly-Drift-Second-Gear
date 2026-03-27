using System;

namespace Assets.Scripts {
    public class Gear {
        public static float g1 = 2.66f;
        public static float g2 = 1.78f;
        public static float g3 = 1.3f;
        public static float g4 = 1f;
        public static float g5 = 0.74f;
        public static float g6 = 0.5f;
        public static float gR = 2.9f;
        public static float x_d = 3.42f;

        public static ValueTuple<int, int>[] rpmTorque = {
            new(1000, 290),
            new(2000, 325),
            new(3000, 335),
            new(3500, 345),
            new(4000, 350),
            new(4500, 355),
            new(5000, 347),
            new(5400, 330),
            new(5650, 300),
            new(6000, 280)
        };

        public static int LookupTorqueCurve(int rpm) {
            if (rpm > 6000) {
                return 280;
            }

            for (var i = 0; i < rpmTorque.Length; i++) {
                if (i >= rpmTorque.Length) {
                    return rpmTorque[rpmTorque.Length].Item2;
                }

                var num = (float)rpmTorque[i].Item1;
                var num2 = (float)rpmTorque[i + 1].Item1;
                if (rpm >= num && rpm < num2) {
                    var num3 = (float)rpmTorque[i].Item2;
                    var num4 = rpmTorque[i + 1].Item2 - num3;
                    var num5 = 1f - (num2 - rpm) / (num2 - num);
                    return (int)(num3 + num4 * num5);
                }
            }

            return 290;
        }
    }
}