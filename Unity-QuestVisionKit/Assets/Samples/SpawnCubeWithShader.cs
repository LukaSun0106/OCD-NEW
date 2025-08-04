using UnityEngine;
using UnityEngine.XR;
using System.Collections.Generic; // Added missing import

public class SpawnCubeWithShader : MonoBehaviour
{
    public Material[] cubeMaterials; // Assign 4 materials in the Inspector, each using a different shader
    [Range(0,3)]
    public int selectedMaterialIndex = 0; // Choose which material/shader to use
    public Vector3 cubeScale = Vector3.one;

    private InputDevice rightController;
    private bool wasButtonPressed = false;

    void Start()
    {
        // Get the right hand controller
        var rightHandedControllers = new List<InputDevice>();
        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
        if (rightHandedControllers.Count > 0)
            rightController = rightHandedControllers[0];
    }

    void Update()
    {
        if (!rightController.isValid)
        {
            // Try to reacquire the controller if lost
            var rightHandedControllers = new List<InputDevice>();
            InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandedControllers);
            if (rightHandedControllers.Count > 0)
                rightController = rightHandedControllers[0];
            return;
        }

        // Check for primary button press (e.g., 'A' on Oculus)
        bool isPressed = false;
        if (rightController.TryGetFeatureValue(CommonUsages.primaryButton, out isPressed) && isPressed)
        {
            if (!wasButtonPressed)
            {
                // Get controller position
                Vector3 position;
                if (rightController.TryGetFeatureValue(CommonUsages.devicePosition, out position))
                {
                    SpawnCube(position);
                }
            }
            wasButtonPressed = true;
        }
        else
        {
            wasButtonPressed = false;
        }
    }

    void SpawnCube(Vector3 position)
    {
        GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        cube.transform.position = position;
        cube.transform.localScale = cubeScale;
        if (cubeMaterials != null && cubeMaterials.Length > selectedMaterialIndex)
        {
            var renderer = cube.GetComponent<Renderer>();
            renderer.material = cubeMaterials[selectedMaterialIndex];
        }
    }
} 