using UnityEngine;

public class CheckPoints : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        SaveSystem.instance.Save();
    }
}
