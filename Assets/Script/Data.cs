using UnityEngine;

namespace Assets.Script
{
    public class Data : MonoBehaviour
    {
        public static int scoreValue;
        public static int playGameValue;
        public static string nameValue;

        // Hàm này có thể được gọi từ bất kỳ đâu để cập nhật giá trị
        public static void UpdateValues(int score, int playGame, string name)
        {
            scoreValue = score;
            playGameValue = playGame;
            nameValue = name;
        }
    }
}
