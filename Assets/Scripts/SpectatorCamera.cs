using UnityEngine;

public class SpectatorCamera : MonoBehaviour
{
    float sensitivity = 1;
    float speed_slow = 25;
    float speed_reg = 50;
    float speed_fast = 100;
    float speed_cur;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(1))
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            // movement
            Vector3 input = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
            if(Input.GetKey(KeyCode.LeftShift))
                speed_cur = speed_fast;
            else if(Input.GetKey(KeyCode.LeftControl))
                speed_cur = speed_slow;
            else
                speed_cur = speed_reg;
            transform.Translate(input* speed_cur * Time.deltaTime);

            // rotation
            Vector3 mouseInput = new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0f);
            transform.Rotate(mouseInput * sensitivity * Time.deltaTime*50);
            Vector3 euler = transform.rotation.eulerAngles;
            transform.rotation = Quaternion.Euler(euler.x, euler.y, 0);
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
