
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class DetectorDispositivo : MonoBehaviour
{
    [SerializeField] private TMP_Text textoDispositivo;
    [SerializeField] private PlayerController jugador;

    private void OnEnable()
    {
        if (jugador == null)
            jugador = FindFirstObjectByType<PlayerController>();

        if (jugador == null)
        {
            Debug.LogError("No se ha encontrado el PlayerController.");
            return;
        }

        jugador.DispositivoUsado += ActualizarTexto;
    }

    private void OnDisable()
    {
        if (jugador != null)
            jugador.DispositivoUsado -= ActualizarTexto;
    }

    private void ActualizarTexto(string dispositivo)
    {
        if (textoDispositivo != null)
            textoDispositivo.text = "Control activo: " + dispositivo;
    }
}
