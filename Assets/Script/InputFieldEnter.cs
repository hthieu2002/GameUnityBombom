using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using Assets.Script;
using UnityEngine.Events;
using System.Collections.Generic;

public class InputFieldEnter : MonoBehaviour
{
    public InputField inputField;

    public UnityEvent<string> onEnterKeyPressed;

    private void Start()
    {
        if (inputField == null)
        {
            // Tự động tìm InputField nếu không được gán
            inputField = GetComponent<InputField>();
        }

        // Thêm sự kiện cho sự kiện kết thúc chỉnh sửa
        inputField.onEndEdit.AddListener(OnEndEdit);
    }

    private void OnEndEdit(string value)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            // Tạo đối tượng GameData và gán giá trị từ Data
            GameData gameData = new GameData();
            gameData.scoreValue = Data.scoreValue;
            gameData.playGameValue = Data.playGameValue;
            gameData.nameValue = Data.nameValue;

            // Đường dẫn đến thư mục chứa file JSON
            string jsonFolderPath = @"F:\github\BomberMan_Unity\json";

            // Đường dẫn đến file JSON
            string filePath = Path.Combine(jsonFolderPath, "gameData.json");

            // Đọc dữ liệu từ tệp JSON hiện có, hoặc tạo một danh sách mới nếu tệp không tồn tại
            List<GameData> gameDataList;
            if (File.Exists(filePath))
            {
                string jsonData = File.ReadAllText(filePath);
                gameDataList = JsonUtility.FromJson<List<GameData>>(jsonData);
            }
            else
            {
                gameDataList = new List<GameData>();
            }

            // Thêm đối tượng GameData mới vào danh sách
            gameDataList.Add(gameData);

            // Chuyển danh sách các đối tượng GameData thành chuỗi JSON
            string jsonDataToWrite = JsonUtility.ToJson(gameDataList);

            // Ghi chuỗi JSON vào tệp
            File.WriteAllText(filePath, jsonDataToWrite);

            SceneManager.LoadScene(0);
        }
    }
}
