using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClickParticleSpawnerUI : MonoBehaviour
{
    [SerializeField] private ParticleSystem particlePrefab;
    [SerializeField] private  Canvas canvas;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && particlePrefab && Cursor.visible)
        {
            SpawnParticleAtMousePosition();
        }
    }

    private void SpawnParticleAtMousePosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out var anchoredPosition
        );
        
        ParticleSystem spawnedParticle = Instantiate(particlePrefab, canvas.transform);
        spawnedParticle.transform.localPosition = anchoredPosition;
        
        spawnedParticle.Play();
        
        Destroy(spawnedParticle.gameObject, spawnedParticle.main.duration);
    }
}
