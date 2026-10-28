using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float pickUpRange = 3f;
    public LayerMask itemLayer;

    public Transform holdPosition;
    public GameObject currentItem;
    public TMP_Text propmtText;
    public Camera playerCamera;

    void Update()
    {
        if (propmtText != null)
        {
            propmtText.text = "";
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentItem != null)
            {
                DropItem(); // Đang cầm thì thả ra
            }
            else
            {
                PickUpItem(); // Tay không thì nhặt vào
            }
        }
    }

    void PickUpItem()
    {
        Camera cameraToUse = playerCamera != null ? playerCamera : Camera.main;
        if (cameraToUse == null)
        {
            Debug.LogError("PlayerInteract requires a player camera or a camera tagged MainCamera.", this);
            return;
        }

        Ray ray = new Ray(cameraToUse.transform.position, cameraToUse.transform.forward);
        RaycastHit hit;
        int raycastMask = itemLayer.value == 0 ? Physics.DefaultRaycastLayers : itemLayer.value;

        if (Physics.Raycast(ray, out hit, pickUpRange, raycastMask))
        {
            if (hit.collider.CompareTag("Item"))
            {
                currentItem = hit.collider.gameObject;
                if (propmtText != null)
                {
                    propmtText.text = "E";
                }

                // 1. Tắt Rigidbody (kinematic)
                Rigidbody rb = currentItem.GetComponent<Rigidbody>();
                if (rb != null)
                {
                    rb.isKinematic = true;
                }

                // 2. LUÔN TẮT COLLIDER để tránh va chạm làm bay nhân vật (Dù có hay không có holdPosition)
                Collider col = currentItem.GetComponent<Collider>();
                if (col != null)
                {
                    col.enabled = false;
                }

                // 3. Gắn vào vị trí cầm
                if (holdPosition != null)
                {
                    currentItem.transform.SetParent(holdPosition);
                    currentItem.transform.localPosition = Vector3.zero;
                    currentItem.transform.localRotation = Quaternion.identity;
                }

                Debug.Log("Đã nhặt: " + currentItem.name);
            }
        }
    }

    void DropItem()
    {
        ReleaseCurrentItem(true);
    }

    void OnDisable()
    {
        ReleaseCurrentItem(false);
    }

    void ReleaseCurrentItem(bool throwItem)
    {
        if (currentItem == null)
        {
            return;
        }

        GameObject item = currentItem;
        currentItem = null;
        Debug.Log("Đã thả: " + item.name);

        // Bật lại Collider khi thả
        Collider col = item.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Bật lại Rigidbody khi thả
        Rigidbody rb = item.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Tách khỏi phân cấp parent
        item.transform.SetParent(null);

        // Đẩy vật phẩm về phía trước một chút
        if (throwItem && rb != null)
        {
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }
    }
}