using UnityEngine;
using UnityEngine.UI;

public class ElementSelectionController : MonoBehaviour
{
	public GameObject player;
	public GameObject selectionUI; // UI để chọn nguyên tố
	public Button fireButton;
	public Button windButton;

	private FireElementController fireElementController;
	private WindControler windElementController;

	void Start()
	{
		// Lấy các component từ player
		fireElementController = player.GetComponent<FireElementController>();
		windElementController = player.GetComponent<WindControler>();

		// Vô hiệu hóa cả hai component khi bắt đầu
		fireElementController.enabled = false;
		windElementController.enabled = false;

		// Hiển thị UI chọn nguyên tố
		selectionUI.SetActive(true);

		// Thêm sự kiện cho các nút
		fireButton.onClick.AddListener(SelectFireElement);
		windButton.onClick.AddListener(SelectWindElement);
	}

	void SelectFireElement()
	{
		fireElementController.enabled = true;
		windElementController.enabled = false;
		selectionUI.SetActive(false);

		// Hiển thị nhân vật nếu bị ẩn
		player.SetActive(true);
	}

	void SelectWindElement()
	{
		fireElementController.enabled = false;
		windElementController.enabled = true;
		selectionUI.SetActive(false);

		// Hiển thị nhân vật nếu bị ẩn
		player.SetActive(true);
	}
}
