using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PianoSetting : MonoBehaviour {
    public GameObject PianoField;
    private bool isSetup = false;
    public TextAsset pianoData;
    public GameObject noteBlock;
    public GameObject standardNote;
    public Material halfMaterial;
    NoteTrigger notetrig;
    Color defaultColor;
	// Use this for initialization
	void Start () {
       notetrig = standardNote.GetComponent<NoteTrigger>();
        defaultColor = standardNote.GetComponent<Renderer>().material.color;
        Debug.Log(defaultColor);
	}
	
	// Update is called once per frame
	void Update () {
        if (!isSetup) {
            PianoField.transform.position = ArtanHololensManager.Instance.GazePosition;
            PianoField.transform.rotation = Camera.main.transform.rotation;
        }else {

        }
        if (Input.GetKeyDown(KeyCode.Space)&&!isSetup) {
            isSetup = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            standardNote.GetComponent<MeshRenderer>().enabled = false;
            standardNote.GetComponent<BoxCollider>().enabled = false;
            StartCoroutine(PianoCoroutine());
            
            StartCoroutine(wait(8f));
           
        }
        

	}

    IEnumerator PianoCoroutine() {
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for(; lineNum<lineData.Length ; lineNum++) {
            if (lineData[lineNum].StartsWith("#")) {
                float size = float.Parse(lineData[lineNum].Substring(1));
                yield return new WaitForSecondsRealtime(0.1f * size);
                continue;
            }
            string[] noteData = lineData[lineNum].Split('/');
            float length=0;
            for(int noteNum = 0 ; noteNum<noteData.Length ; noteNum++) {
                string[] note = noteData[noteNum].Split(',');
                float offset = float.Parse(note[0]);
                float size = float.Parse(note[1]);
                length = size;
                GameObject newNote = Instantiate(noteBlock, this.transform) as GameObject;
                newNote.transform.localPosition = standardNote.transform.localPosition;
                newNote.transform.localRotation = standardNote.transform.localRotation;
                newNote.transform.Translate(new Vector3((1f / 38f) * offset, 1f, 0), Space.Self);
                newNote.transform.localScale = new Vector3(1f / 38f, (1f / 16f) * size, 1);
                newNote.transform.Translate(new Vector3(0, newNote.transform.lossyScale.y/2f,0), Space.Self);
                if (offset - Mathf.Floor(offset) > 0)
                    newNote.GetComponent<Renderer>().material = halfMaterial;
            }
            yield return new WaitForSecondsRealtime(0.2f*length);
        }
        yield return null;
    }
    IEnumerator ReadyCoroutine()
    {
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for (; lineNum < lineData.Length; lineNum++)
        {
            if (lineData[lineNum].StartsWith("#"))
            {
                float size = float.Parse(lineData[lineNum].Substring(1));
                yield return new WaitForSecondsRealtime(0.1f * size);
                continue;
            }
            string[] noteData = lineData[lineNum].Split('/');
            float length = 0;
            for (int noteNum = 0; noteNum < noteData.Length; noteNum++)
            {
                
                string[] note = noteData[noteNum].Split(',');
                float offset = float.Parse(note[0]);
                float size = float.Parse(note[1]);
                length = size;
                if (offset - Mathf.Floor(offset) > 0)
                {

                }
                else
                {
                    if (notetrig.keyboard[(int)offset + 20].GetComponent<Renderer>().material.color == defaultColor)
                    {
                        notetrig.keyboard[(int)offset + 20].GetComponent<Renderer>().material.color = Color.yellow;
                        StartCoroutine(backColor((int)offset + 20, length));
                    }
                    else
                    {
                        StartCoroutine(backColor((int)offset + 20, length));
                    }
                }
            }

            yield return new WaitForSecondsRealtime(0.2f * length);
        }
        yield return null;
    }
    IEnumerator backColor(int offset, float length)
    {
        
        yield return StartCoroutine(lerpColor(offset,length));
        notetrig.keyboard[offset].GetComponent<Renderer>().material.color = defaultColor;
    }
    IEnumerator lerpColor(int offset, float length)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            notetrig.keyboard[offset].GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, elapsedTime/1f);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator wait(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(ReadyCoroutine());
    }
}
