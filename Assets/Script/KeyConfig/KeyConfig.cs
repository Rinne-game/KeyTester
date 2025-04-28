using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyConfig : MonoBehaviour
{
    [System.Serializable]
    public class KeyData_Main
    {
        public KeyCode[] KeyConfig;
    }
    public KeyData_Main KeyData;
    public Button[] Button_Key;
    public Text[] Key_text;
    private string[] text;
    public bool islockkey;
    public Slider[] Stick;
    // Start is called before the first frame update
    void Start()
    {
        KeyData_Main temp = new KeyData_Main();
        temp.KeyConfig = new KeyCode[6];
        text = new string[6];
        KeyData = JsonUtility.FromJson<KeyData_Main>(PlayerPrefs.GetString("Keyconfig", JsonUtility.ToJson(temp)));
        //KeyData = temp;
        if (KeyData.KeyConfig == null)
        {
            KeyData.KeyConfig = new KeyCode[6];
        }
        islockkey = false;
        for (int i = 0; i < Button_Key.Length; i++)
        {
            Button_Key[i].interactable = true;
        }
        for (int i = 0; i < Key_text.Length; i++)
        {
            Key_text[i].text = $"Key{i + 1}:{KeyData.KeyConfig[i]}";
            text[i] = $"Key{i + 1}:{KeyData.KeyConfig[i]}";
        }

    }
    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < Key_text.Length; i++)
        {
            if (Input.GetKey(KeyData.KeyConfig[i]))
            {
                Key_text[i].text = $"<color=#ff0000>{text[i]}</color>";
            }
            else
            {
                Key_text[i].text = text[i];
            }
        }
        if (Input.GetKey(KeyCode.Escape) && Input.GetKey(KeyCode.Backspace))
        {
            Key_Reset();
        }
        for (int i = 0; i < Stick.Length; i++)
        {
            Stick[i].minValue = -1;
            Stick[i].maxValue = 1;
            // Input.GetAxis
            switch (i)
            {
                case 0:
                    Stick[i].value = Input.GetAxis("Vertical");
                    break;
                case 1:
                    Stick[i].value = Input.GetAxis("Horizontal");
                    break;
            }
        }

    }
    public void Keycode_onclick(int value)
    {
        Debug.Log($"KeyDataID:{value}");
        if (value < 6 && value >= 0)
        {
            for (int i = 0; i < Button_Key.Length; i++)
            {
                Button_Key[i].interactable = false;
            }
            StartCoroutine(Keycode_Setting(value));
        }
    }
    IEnumerator Keycode_Setting(int value)
    {
        islockkey = true;
        text[value] = $"Key{value + 1}:Setting";
        // 任意のキーが押されるまでループ
        while (true)
        {
            if (Input.anyKeyDown)
            {
                // 入力されたキーを取得
                foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
                {
                    if (Input.GetKeyDown(keyCode))
                    {
                        // 入力されたキーを表示
                        Debug.Log("Key pressed: " + keyCode);
                        KeyData.KeyConfig[value] = keyCode;
                        islockkey = true;
                        PlayerPrefs.SetString("Keyconfig", JsonUtility.ToJson(KeyData));
                        for (int i = 0; i < Button_Key.Length; i++)
                        {
                            Button_Key[i].interactable = true;
                        }
                        text[value] = $"Key{value + 1}:{KeyData.KeyConfig[value]}";

                        // ここにキーが押された後の処理を追加
                    }
                }
                yield break; // キーが押されたらコルーチンを終了
            }
            yield return null; // 次のフレームまで待つ
        }
    }
    public void Key_Reset()
    {
        for (int i = 0; i < KeyData.KeyConfig.Length; i++)
        {
            KeyData.KeyConfig[i] = new KeyCode();
            text[i] = $"Key{i + 1}:{KeyData.KeyConfig[i]}";
        }
        PlayerPrefs.SetString("Keyconfig", JsonUtility.ToJson(KeyData));
    }
}
