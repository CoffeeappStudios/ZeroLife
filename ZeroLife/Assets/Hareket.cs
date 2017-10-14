using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hareket : MonoBehaviour {

    float aci;
    bool ziplamaIzin, kutuTasiniyor;
    public Collider2D kutuCol;
    public GameObject takeButton;
    public LayerMask kutununLayeri;

	void Update () {
        transform.position += new Vector3(Input.GetAxis("Horizontal"), 0, 0) * Time.deltaTime * 3;
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.GetComponent<Animator>().SetFloat("hiz", 1);
        }
        else
        {
            transform.GetComponent<Animator>().SetFloat("hiz", 0);
        }
        if (Input.GetKeyDown(KeyCode.W) && ziplamaIzin == true)
        {
            transform.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, 10) * Time.deltaTime * 1700);
            ziplamaIzin = false;
        }
        if (Physics2D.OverlapCircle(transform.position, 1.5f, kutununLayeri))
        {
            takeButton.SetActive(true);
            kutuCol = Physics2D.OverlapCircle(transform.position, 1.5f, kutununLayeri);
        }
        else if(kutuTasiniyor == false)
        {
            takeButton.SetActive(false);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.tag == "Zemin")
        {
            ziplamaIzin = true;
        }
    }

    public void KutuTasima()
    {
        if(kutuCol != null && kutuTasiniyor == false)
        {
            kutuTasiniyor = true;
            kutuCol.gameObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;
            kutuCol.gameObject.GetComponent<BoxCollider2D>().enabled = false;
            if(kutuCol.transform.position.x < transform.position.x)
            {
                kutuCol.transform.position = new Vector3(transform.position.x - 1f, transform.position.y);
            }
            else if (kutuCol.transform.position.x >= transform.position.x)
            {
                kutuCol.transform.position = new Vector3(transform.position.x + 1f, transform.position.y);
            }
            takeButton.GetComponentInChildren<Text>().text = "Bırak";
            StartCoroutine(KutuTasimacisi(kutuCol.gameObject));
        }
        else
        {
            kutuTasiniyor = false;
            kutuCol.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            kutuCol.gameObject.GetComponent<BoxCollider2D>().enabled = true;
            takeButton.GetComponentInChildren<Text>().text = "Taşı";
            StopCoroutine(KutuTasimacisi(kutuCol.gameObject));
        }
    }

    IEnumerator KutuTasimacisi(GameObject kutu)
    {
        if(Input.GetAxis("Horizontal") > 0)
        {
            kutu.transform.position = new Vector3(transform.position.x + 1f, transform.position.y);
        }
        else if (Input.GetAxis("Horizontal") < 0)
        {
            kutu.transform.position = new Vector3(transform.position.x - 1f, transform.position.y);
        }
        else if (Input.GetAxis("Horizontal") == 0)
        {
            if (kutuCol.transform.position.x < transform.position.x)
            {
                kutuCol.transform.position = new Vector3(transform.position.x - 1f, transform.position.y);
            }
            else if (kutuCol.transform.position.x >= transform.position.x)
            {
                kutuCol.transform.position = new Vector3(transform.position.x + 1f, transform.position.y);
            }
        }
        yield return new WaitForSeconds(0.01f);
        if(kutuTasiniyor == true)
        {
            StartCoroutine(KutuTasimacisi(kutu));
        }
    }
}
