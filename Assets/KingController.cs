using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface IKillable
{
    [PunRPC]
    void Kill();
}

public class KingController : MonoBehaviour
{
    [SerializeField] GameObject kingCamera;
    [SerializeField] GameObject groundCamera;
    [SerializeField] GameObject groundAnchor;

    [SerializeField] GameObject highCanvas;
    [SerializeField] GameObject groundCanvas;

    [SerializeField] float groundCameraMoveSpeed = 0.004f;
    [SerializeField] float groundCameraDeadzone = 3f;

    [SerializeField] PhotonView photonView;

    //public float mouseDistanceFromOrigin;

    GameObject currentCamera;

    public bool isObserving = false;

    // Start is called before the first frame update
    void Start()
    {
        if(photonView.IsMine)
        {
            kingCamera.SetActive(true);
            currentCamera = kingCamera;
            groundCamera.SetActive(false);
        }
        else
        {
            this.gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            // Left click to kill >:(
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit2D hit = Physics2D.Raycast(GetMousePosition(), Vector2.zero);
                if(hit.collider != null)
                {
                    Debug.Log(hit.collider.gameObject.name);
                    IKillable victim = hit.transform.gameObject.GetComponent<IKillable>();
                    if(victim != null)
                    {
                        if(PhotonNetwork.IsConnected && PhotonNetwork.InRoom)
                        {
                            hit.transform.gameObject.GetComponent<PhotonView>().RPC("Kill", RpcTarget.All);
                        }
                        else
                        {
                            victim.Kill();
                        }
                    }
                }
            
            }

            // Right click to zoom in / out
            if(Input.GetMouseButtonDown(1))
            {
                isObserving = !isObserving;

                if (isObserving)
                {
                    groundAnchor.transform.position = GetMousePosition();

                    kingCamera.SetActive(false);
                    highCanvas.SetActive(false);

                    groundCamera.SetActive(true);
                    currentCamera = groundCamera;

                }
                else
                {
                    kingCamera.SetActive(true);
                    groundCamera.SetActive(false);
                    currentCamera = kingCamera;
                    highCanvas.SetActive(true);
                }
            }

            if(isObserving)
            {
                Vector2 mousePos = GetMousePosition();
                float mouseDistanceFromOrigin = Vector2.Distance(groundAnchor.transform.position, mousePos);

                if (mouseDistanceFromOrigin > groundCameraDeadzone)
                {
                    groundAnchor.transform.position = Vector2.Lerp(groundAnchor.transform.position, mousePos, groundCameraMoveSpeed);
                }
            }
        }
    }

    Vector2 GetMousePosition()
    {
        Camera camera = currentCamera.GetComponent<Camera>();
        if (camera != null)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            return new Vector2(mousePos.x, mousePos.y);
        }
        else return Vector2.zero;
    }
}
