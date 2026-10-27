using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public float pickUpRange = 3f;
    public LayerMask itemLayer;

    public Transform holdPosition;
    public GameObject currentItem;

    void Update()
    {
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
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, pickUpRange, itemLayer))
        {
            if (hit.collider.CompareTag("Item"))
            {
                currentItem = hit.collider.gameObject;

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
        Debug.Log("Đã thả: " + currentItem.name);

        // Bật lại Collider khi thả
        Collider col = currentItem.GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }

        // Bật lại Rigidbody khi thả
        Rigidbody rb = currentItem.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = false;
        }

        // Tách khỏi phân cấp parent
        currentItem.transform.SetParent(null);

        // Đẩy vật phẩm về phía trước một chút
        if (rb != null)
        {
            rb.AddForce(transform.forward * 5f, ForceMode.Impulse);
        }

        currentItem = null;
    }
}