using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ID_ForcesForAcceleration_Demo : MonoBehaviour
{
    private ArticulationBody ab;
    public float desiredAcceleration = 0.5f;
    public Text textPanel;

    void Start()
    {
        ab = GetComponent<ArticulationBody>();
    }

    void FixedUpdate()
    {

        List<float> gravity = new();
        List<float> coriolisCentrifugal = new();
        List<int> indices = new();

        ab.GetJointGravityForces(gravity);
        ab.GetDofStartIndices(indices);
        ab.GetJointCoriolisCentrifugalForces(coriolisCentrifugal);

        ArticulationReducedSpace desired = new(Mathf.Deg2Rad * desiredAcceleration);

        ArticulationReducedSpace accelerationForces = ab.GetJointForcesForAcceleration(desired);

        ArticulationReducedSpace totalForce = new(accelerationForces[0] + gravity[indices[ab.index]] + coriolisCentrifugal[indices[ab.index]]);

        ab.jointForce = totalForce;

        List<string> lines = new();
        lines.Add($"Desired Acceleration: {desiredAcceleration:F4} [º/s]  /  {Mathf.Deg2Rad * desiredAcceleration:F4} [rad/s] ");
        lines.Add("");
        lines.Add($"Current Position: {Mathf.Rad2Deg * ab.jointPosition[0]:F4} [º]  /  {ab.jointPosition[0]:F4} [rad] ");
        lines.Add($"Current Velocity: {Mathf.Rad2Deg * ab.jointVelocity[0]:F4} [º/s]  /  {ab.jointVelocity[0]:F4} [rad/s] ");
        lines.Add($"Current Acceleration: {Mathf.Rad2Deg * ab.jointAcceleration[0]:F4} [º/s]  /  {ab.jointAcceleration[0]:F4} [rad/s] ");
        lines.Add("");
        lines.Add($"Gravity Torque: {gravity[indices[ab.index]]:F4} [N•m] ");
        lines.Add($"Acceleration Torque: {accelerationForces[0]:F4} [N•m] ");
        lines.Add($"Coriolis / Centrifugal Torque: {coriolisCentrifugal[indices[ab.index]]:F4} [N•m] ");
        lines.Add($"Total Torque: {totalForce[0]:F4} [N•m] ");

        textPanel.text = string.Join("\n", lines);
    }
}

