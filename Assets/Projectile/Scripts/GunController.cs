using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private Projectile bulletPrefab, rocketPrefab;
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var camPos = Camera.main.transform.position;
                var dir = hit.point - camPos;
                Instantiate(bulletPrefab, camPos, Quaternion.LookRotation(dir));
            }
        }
        if (Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out var hit))
            {
                var camPos = Camera.main.transform.position;
                var dir = hit.point - camPos;
                Instantiate(rocketPrefab, camPos, Quaternion.LookRotation(dir));
            }
        }
    }
}
