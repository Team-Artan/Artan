using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class PianoSetting : MonoBehaviour {
    public GameObject PianoField;
    private bool isSetup = false;
    public TextAsset[] ScoreBoard;
    TextAsset pianoData;
    public GameObject noteBlock;
    public GameObject standardNote;
    public GameObject Bar;
    public GameObject BarStart;
    public Material halfMaterial;
    NoteTrigger notetrig;
    bool scoreReady = false;
    GameObject cursor;
    Color defaultColor;
    float MusicTime;
    Vector3 barLastPos;
    Vector3 barLastScale;
    // Use this for initialization
    void Start()
    {
        barLastPos = Bar.transform.localPosition;
        barLastScale = Bar.transform.localScale;
        Bar.transform.localScale = new Vector3(0, Bar.transform.localScale.y, Bar.transform.localScale.z);
        Bar.transform.localPosition = BarStart.transform.localPosition;

        cursor = GameObject.Find("CursorWithFeedback");
        notetrig = standardNote.GetComponent<NoteTrigger>();
        defaultColor = standardNote.GetComponent<Renderer>().material.color;
        Debug.Log(defaultColor);
    }
	
	// Update is called once per frame
	void Update () {
        if (!isSetup) {
            PianoField.transform.position = ArtanHololensManager.Instance.GazePosition;
            PianoField.transform.rotation = Quaternion.Euler(75, Camera.main.transform.rotation.eulerAngles.y, Camera.main.transform.rotation.eulerAngles.z);


        }
        else {

        }
        if ((ArtanHololensManager.Instance.Tapped||Input.GetKeyDown(KeyCode.Space))&&!isSetup) {
            isSetup = true;
            this.GetComponent<MeshRenderer>().enabled = false;
            standardNote.GetComponent<MeshRenderer>().enabled = false;
            standardNote.GetComponent<BoxCollider>().enabled = false;
        }
        if (pianoData != null && scoreReady == false)
        {
            scoreReady = true;

            StartCoroutine(PianoCoroutine());
            StartCoroutine(wait(7.8f));

        }
        

	}
    public void together()
    {
        pianoData = ScoreBoard[0];
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for (; lineNum < lineData.Length; lineNum++)
        {
            if (lineData[lineNum].StartsWith("#"))
            {
                float size = float.Parse(lineData[lineNum].Substring(1));
                MusicTime += 0.2f * size;
            }
            string[] noteData = lineData[lineNum].Split('/');
            float length = 0;
            for (int noteNum = 0; noteNum < noteData.Length; noteNum++)
            {

                string[] note = noteData[noteNum].Split(',');
                float offset = float.Parse(note[0]);
                float size = float.Parse(note[1]);
                length = size;
                MusicTime += 0.2f * length;
            }
        }
    }
    public void Star()
    {
        pianoData = ScoreBoard[1];
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for (; lineNum < lineData.Length; lineNum++)
        {
            if (lineData[lineNum].StartsWith("#"))
            {
                float size = float.Parse(lineData[lineNum].Substring(1));
                MusicTime += 0.2f * size;
            }
            string[] noteData = lineData[lineNum].Split('/');
            float length = 0;
            for (int noteNum = 0; noteNum < noteData.Length; noteNum++)
            {

                string[] note = noteData[noteNum].Split(',');
                float offset = float.Parse(note[0]);
                float size = float.Parse(note[1]);
                length = size;
                MusicTime += 0.2f * length;
            }
        }
    }
    public void Fish()
    {
        pianoData = ScoreBoard[2];
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for (; lineNum < lineData.Length; lineNum++)
        {
            if (lineData[lineNum].StartsWith("#"))
            {
                float size = float.Parse(lineData[lineNum].Substring(1));
                MusicTime += 0.2f * size;
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
                MusicTime += 0.2f * length;
            }
        }
        Debug.Log(MusicTime);
    }
    public void Restart()
    {
        SceneManager.LoadScene("PianoScene",LoadSceneMode.Single);   
    }
    IEnumerator PianoCoroutine() {
        string[] lineData = pianoData.text.Split('\n');
        int lineNum = 0;
        for(; lineNum<lineData.Length ; lineNum++) {
            if (lineData[lineNum].StartsWith("#")) {
                float size = float.Parse(lineData[lineNum].Substring(1));
                yield return new WaitForSecondsRealtime(0.2f * size);
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
                newNote.transform.Translate(new Vector3((1f / 36f / 1.2f) * offset, 1f, 0), Space.Self);
                newNote.transform.localScale = new Vector3(1f / 36f, (1f / 16f) * size, 1);
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
                yield return new WaitForSecondsRealtime(0.2f * size);
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
                    if(notetrig.keyboard[(int)offset+20].transform.localScale.y == 0)
                        StartCoroutine(backSize((int)offset + 20, 1f));
                    else
                    {
                        //GameObject temp = Instantiate(noteBlock, transform) as GameObject;
                        //temp.transform.SetParent(this.transform);
                        //temp.GetComponent<Renderer>().material.color = Color.cyan;
                        //temp.transform.localScale = new Vector3(temp.transform.localScale.x, 0, temp.transform.localScale.z);
                        ////temp.transform.localRotation = transform.localRotation;
                        ////temp.transform.localPosition = notetrig.keyboard[(int)offset + 20].transform.localPosition + new Vector3(0,0,-0.1f);
                        //temp.transform.localPosition = new Vector3(notetrig.keyboard[(int)offset + 20].transform.localPosition.x, 0, notetrig.keyboard[(int)offset + 20].transform.localPosition.z);
                        StartCoroutine(backTemp((int)offset + 20, 1f));
                    }
                }
            }

            yield return new WaitForSecondsRealtime(0.2f * length);
        }
        yield return null;
    }
    IEnumerator ReadyCoroutine2()
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

                        StartCoroutine(backColor((int)offset + 20, 1f));
                }
            }

            yield return new WaitForSecondsRealtime(0.2f * length);
        }
        yield return null;
    }
    IEnumerator backColor(int offset, float length)
    {
        yield return StartCoroutine(lerpColor(offset,length));
        notetrig.keyboard_color[offset].GetComponent<Renderer>().material.color = defaultColor;
    }
    IEnumerator lerpColor(int offset, float length)
    {
        float elapsedTime = 0;
        while (elapsedTime < 1f)
        {
            notetrig.keyboard_color[offset].GetComponent<Renderer>().material.color = Color.Lerp(Color.red, Color.green, elapsedTime/1f);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator backSize(int offset, float length)
    {
        Vector3 formerSize = new Vector3(notetrig.keyboard[offset].transform.localScale.x, 0, notetrig.keyboard[offset].transform.localScale.z);
        Vector3 formerPos = new Vector3(notetrig.keyboard[offset].transform.localPosition.x, -0.5f, notetrig.keyboard[offset].transform.localPosition.z);
        yield return StartCoroutine(lerpSize(offset, length));
        notetrig.keyboard[offset].transform.localScale = formerSize;
        notetrig.keyboard[offset].transform.localPosition = formerPos;
    }
    IEnumerator lerpSize(int offset, float length)
    {
        float elapsedTime = 0;
        Vector3 formerSize = new Vector3(notetrig.keyboard[offset].transform.localScale.x, 0, notetrig.keyboard[offset].transform.localScale.z);
        Vector3 afterSize = new Vector3(notetrig.keyboard[offset].transform.localScale.x, 1, notetrig.keyboard[offset].transform.localScale.z);
        Vector3 formerPos = new Vector3(notetrig.keyboard[offset].transform.localPosition.x, -0.5f, notetrig.keyboard[offset].transform.localPosition.z);
        Vector3 afterPos = new Vector3(notetrig.keyboard[offset].transform.localPosition.x, 0, notetrig.keyboard[offset].transform.localPosition.z);
        while (elapsedTime < length)
        {
            notetrig.keyboard[offset].transform.localScale = Vector3.Lerp(formerSize, afterSize, elapsedTime / length);
            notetrig.keyboard[offset].transform.localPosition = Vector3.Lerp(formerPos, afterPos, elapsedTime / length);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator lerpTemp( int offset, float length)
    {
        float elapsedTime = 0;
        Vector3 formerSize = new Vector3(notetrig.keyboard2[offset].transform.localScale.x, 0, notetrig.keyboard2[offset].transform.localScale.z);
        Vector3 afterSize = new Vector3(notetrig.keyboard2[offset].transform.localScale.x, 1, notetrig.keyboard2[offset].transform.localScale.z);
        Vector3 formerPos = new Vector3(notetrig.keyboard2[offset].transform.localPosition.x, -0.5f, notetrig.keyboard2[offset].transform.localPosition.z - 0.1f);
        Vector3 afterPos = new Vector3(notetrig.keyboard2[offset].transform.localPosition.x, 0, notetrig.keyboard2[offset].transform.localPosition.z - 0.1f);
        while (elapsedTime < length)
        {
            notetrig.keyboard2[offset].transform.localScale = Vector3.Lerp(formerSize, afterSize, elapsedTime / length);
            notetrig.keyboard2[offset].transform.localPosition = Vector3.Lerp(formerPos, afterPos, elapsedTime / length);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
    IEnumerator backTemp(int offset, float length)
    {
        Vector3 formerSize = new Vector3(notetrig.keyboard2[offset].transform.localScale.x, 0, notetrig.keyboard2[offset].transform.localScale.z);
        Vector3 formerPos = new Vector3(notetrig.keyboard2[offset].transform.localPosition.x, -0.5f, notetrig.keyboard2[offset].transform.localPosition.z);
        yield return StartCoroutine(lerpTemp(offset, length));
        notetrig.keyboard2[offset].transform.localScale = formerSize;
        notetrig.keyboard2[offset].transform.localPosition = formerPos;
    }

    IEnumerator wait(float sec)
    {
        yield return new WaitForSeconds(sec);
        StartCoroutine(ReadyCoroutine());
        StartCoroutine(barLerp());
    }
    IEnumerator barLerp()
    {

        float elapsedTime = 0;
        while(elapsedTime < MusicTime)
        {

            Bar.transform.localPosition = Vector3.Lerp(BarStart.transform.localPosition, barLastPos, elapsedTime / MusicTime);
            Bar.transform.localScale = Vector3.Lerp(new Vector3(0, Bar.transform.localScale.y, Bar.transform.localScale.z), barLastScale, elapsedTime / MusicTime);
            elapsedTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }
}
