using Photon.Pun;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace AmigoChat.Scripts
{
    public class ChatManager : MonoBehaviourPun
    {
        [SerializeField]
        private GameObject _loginPanel, _chatPanel, _exitPanel, _chatMessageContainer, _newChatPanel;

        [SerializeField]
        private TMP_InputField _interestsInput, _chatMsgInput;

        [SerializeField]
        private ChatMessage _chatMessagePrefab;

        [SerializeField]
        private TextMeshProUGUI _reportUserText;

        public static ChatManager Instance;

        PhotonView _photonView;

        // Start is called before the first frame update
        void Start()
        {
            SetLoginScreen();
            Instance = this;
            _photonView = PhotonView.Get(this);
        }

        // Update is called once per frame
        void Update()
        {
        }

        public void OnStartBtnClick()
        {
            ConnectAmigo();
        }

        void ConnectAmigo()
        {
            string interests = string.IsNullOrEmpty(_interestsInput.text) ? null : _interestsInput.text;
            RoomManager.Instance.JoinOrCreateRoom(interests);
            _loginPanel.SetActive(false);
            _chatPanel.SetActive(true);
        }

        public void OnSendBtnClick()
        {
            ReportUser("");
            string chatMsg = _chatMsgInput.text;
            _chatMsgInput.text = "";
            if (!string.IsNullOrEmpty(chatMsg))
            {
                SendChatMessage(chatMsg);
            }
        }

        public void ReportUser(string message)
        {
            _reportUserText.text = message;
        }

        public void ClearChat()
        {
            foreach (Transform child in _chatMessageContainer.transform)
            {
                child.gameObject.SetActive(false);
                Destroy(child.gameObject);
            }
        }

        public void ExitToLoginScreen()
        {
            SetLoginScreen();
            ClearChat();
            LeaveChatRoom();
        }

        private void SetLoginScreen()
        {
            _loginPanel.SetActive(true);
            _chatPanel.SetActive(false);
            _exitPanel.SetActive(false);
            _newChatPanel.SetActive(false);
        }

        private void LeaveChatRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void SendChatMessage(string messageText)
        {
            if (messageText != "")
            {
                _photonView.RPC("ReceiveMessage", RpcTarget.Others, messageText);
                AddMessageToChat("You", messageText);
            }
        }

        [PunRPC]
        void ReceiveMessage(string message)
        {
            AddMessageToChat("Amigo", message);
        }

        void AddMessageToChat(string author, string message)
        {
            ChatMessage chatMessage = Instantiate(_chatMessagePrefab);
            chatMessage.Author = author;
            chatMessage.Message = message;
            chatMessage.transform.SetParent(_chatMessageContainer.transform);
            chatMessage.transform.localScale = Vector3.one;
            Vector3 chatMsgPos = chatMessage.transform.localPosition;
            chatMsgPos.x -= 20;
            chatMessage.transform.localPosition = chatMsgPos;
        }

        public void FindNewAmigo()
        {
            LeaveChatRoom();
            ConnectAmigo();
        }
    }
}
