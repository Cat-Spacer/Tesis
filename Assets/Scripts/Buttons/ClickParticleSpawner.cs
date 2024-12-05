using UnityEngine;

public class ClickParticleSpawnerUI : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab; // Prefab del sistema de partículas
    [SerializeField] Canvas canvas; // Referencia al Canvas si es necesario

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 es el botón izquierdo del mouse
        {
            SpawnParticleAtMousePosition();
        }
    }

    void SpawnParticleAtMousePosition()
    {
        // Obtener la posición del mouse en el espacio del canvas
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPosition;

        // Convertir posición de pantalla a espacio local del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out anchoredPosition
        );

        // Instanciar el sistema de partículas como hijo del Canvas
        ParticleSystem spawnedParticle = Instantiate(particlePrefab, canvas.transform);
        spawnedParticle.transform.localPosition = anchoredPosition;

        // Activar las partículas
        spawnedParticle.Play();

        // Opcional: destruir las partículas después de un tiempo
        Destroy(spawnedParticle.gameObject, spawnedParticle.main.duration);
    }
}
