using UnityEngine;

public class ClickParticleSpawnerUI : MonoBehaviour
{
    [SerializeField] ParticleSystem particlePrefab; // Prefab del sistema de part�culas
    [SerializeField] Canvas canvas; // Referencia al Canvas si es necesario

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 0 es el bot�n izquierdo del mouse
        {
            SpawnParticleAtMousePosition();
        }
    }

    void SpawnParticleAtMousePosition()
    {
        // Obtener la posici�n del mouse en el espacio del canvas
        Vector2 mousePosition = Input.mousePosition;
        Vector2 anchoredPosition;

        // Convertir posici�n de pantalla a espacio local del Canvas
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            mousePosition,
            canvas.worldCamera,
            out anchoredPosition
        );

        // Instanciar el sistema de part�culas como hijo del Canvas
        ParticleSystem spawnedParticle = Instantiate(particlePrefab, canvas.transform);
        spawnedParticle.transform.localPosition = anchoredPosition;

        // Activar las part�culas
        spawnedParticle.Play();

        // Opcional: destruir las part�culas despu�s de un tiempo
        Destroy(spawnedParticle.gameObject, spawnedParticle.main.duration);
    }
}
