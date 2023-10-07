using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

//Duty:處理左邊的社交按鈕
public class SocialUIButton : MonoBehaviour
{
	// Start is called before the first frame update	[Range(0.0f, 10.0f)]
	public float moveSpeed = 3.0f;


	private Vector2 _hidePos;
	private Vector2 _showPos;
	private RectTransform _rectTransfrom;
	[SerializeField] private scrollrect Scrollrect;
	[SerializeField] private GameObject Chat;
	[SerializeField] private GameObject AutoPlayButton;
	[SerializeField] private GameObject SupportButtom;
	private bool _isShow;

	// Use this for initialization
	void Start()
	{
        _rectTransfrom = this.gameObject.GetComponent<RectTransform>();
        _hidePos = _rectTransfrom.anchoredPosition;
        _showPos = new Vector2(_hidePos.x + Chat.GetComponent<RectTransform>().rect.width, _hidePos.y);
		 _isShow = false;
	}

	// Update is called once per frame
	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (_isShow)
			{
				HideMenu();
			}
			else
			{
				ShowMenu();
			}
		}

	}
	public void showUI()
	{
		if (_isShow)
		{
			HideMenu();
		}
		else
		{
			ShowMenu();
		}
	}

	public void ShowMenu()
	{
		StartCoroutine(Appear());
	}

	public void HideMenu()
	{
		StartCoroutine(Disappear());
	}

	public void AddChat(List<Tuple<String, String>> text)
	{
		Scrollrect.AddChat(text);
	}

	IEnumerator Disappear()
	{

		_isShow = false;

		float time = Time.time;
		float timeDiff = 0;

		while (timeDiff < 1)
		{
			timeDiff = (Time.time - time) * moveSpeed;
			Vector2 currentPos = Vector2.Lerp(_showPos, _hidePos, timeDiff);
			_rectTransfrom.anchoredPosition = currentPos;

			yield return new WaitForEndOfFrame();
		}
		Chat.SetActive(false);
		AutoPlayButton.SetActive(true);
		SupportButtom.SetActive(true);
	}

	IEnumerator Appear()
	{
		Chat.SetActive(true);
		AutoPlayButton.SetActive(false);
		SupportButtom.SetActive(false);

		float time = Time.time;
		float timeDiff = 0;

		while (timeDiff < 1)
		{
			timeDiff = (Time.time - time) * moveSpeed;
			Vector2 currentPos = Vector2.Lerp(_hidePos, _showPos, timeDiff);
			_rectTransfrom.anchoredPosition = currentPos;

			yield return new WaitForEndOfFrame();
		}

		_isShow = true;
	}
}
