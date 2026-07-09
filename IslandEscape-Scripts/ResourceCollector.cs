using UnityEngine;

public class ResourceCollector : MonoBehaviour
{
    public float collectRadius = 2f;
    public LayerMask resourceLayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            CollectResources();
        }
    }

    void CollectResources()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, collectRadius, resourceLayer);
        foreach (var hitCollider in hitColliders)
        {
            // Assuming resource has a Resource type or component
            Resource resource = hitCollider.GetComponent<Resource>();
            if (resource != null)
            {
                // Logic to collect resource
                Debug.Log("Collected: " + resource.resourceName);
                // Implement any inventory or item collection logic here
                Destroy(hitCollider.gameObject); // Optional, destroy the resource after collection
            }
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, collectRadius);
    }
#endif
}
