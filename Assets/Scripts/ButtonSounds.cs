using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonSounds : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Image background;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        GetComponent<AudioSource>().Play();
        background.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        GetComponent<AudioSource>().Stop();
        background.gameObject.SetActive(false);
    }
}
