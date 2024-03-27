using Cinemachine;
using UnityEngine;

public class GetCameraDown : MonoBehaviour
{
    [SerializeField]CinemachineVirtualCamera mainCamera;
    [SerializeField] CinemachineVirtualCamera CameraDown;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainCamera.gameObject.SetActive(false);
            CameraDown.gameObject.SetActive(true);
            CameraDown.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset = mainCamera.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset;
            CameraDown.GetCinemachineComponent<Cinemachine.CinemachineTransposer>().m_FollowOffset.y = -2f;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            mainCamera.gameObject.SetActive(true);
            CameraDown.gameObject.SetActive(false);// Mo�esz dostosowa� t� warto��
        }
    }
}
