"""
Convenience wrapper around mlagents-learn.

Usage:
    uv run train                          # start a new run
    uv run train --run-id=car_v2         # named run
    uv run train --resume --run-id=car_v1  # resume an existing run
"""

import subprocess
import sys


CONFIG = "configs/car_trainer.yaml"
RESULTS_DIR = "results"
DEFAULT_RUN_ID = "car_v1"


def train():
    run_id = DEFAULT_RUN_ID
    extra_args = []

    for arg in sys.argv[1:]:
        if arg.startswith("--run-id="):
            run_id = arg.split("=", 1)[1]
        else:
            extra_args.append(arg)

    cmd = [
        "mlagents-learn",
        CONFIG,
        f"--run-id={run_id}",
        f"--results-dir={RESULTS_DIR}",
        *extra_args,
    ]

    print(f"Running: {' '.join(cmd)}")
    subprocess.run(cmd, check=True)


if __name__ == "__main__":
    train()
