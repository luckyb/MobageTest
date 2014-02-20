using UnityEngine;
using System.Collections.Generic;
using Helpshiftandroid;

public class TestScript : MonoBehaviour {
	private Support help;
	
	
		public void ShowSupport()
	{
		help.setNameAndEmail("rhishikesh", "rhishikesh@helpshift.com");
		help.setUserIdentifier("rhishikeshIdentifier");

		// Call plugin only when running on real device
		int index = 0;
		if (Application.platform != RuntimePlatform.OSXEditor)
		{
			if (index == 0) {
				Dictionary<string, string> configMap = new Dictionary<string, string>();
				configMap.Add("gotoConversationAfterContactUs", "no");
				configMap.Add("enableContactUs", "yes");
				help.showFAQs(configMap);
			} else if (index == 1){
				Dictionary<string, string> configMap = new Dictionary<string, string>();
				configMap.Add("enableContactUs", "yes");
				configMap.Add("gotoConversationAfterContactUs", "yes");
				help.showConversation(configMap);
		    } else if (index == 2) {
				Dictionary<string, string> configMap = new Dictionary<string, string>();
				configMap.Add("gotoConversationAfterContactUs", "yes");
				configMap.Add("enableContactUs", "yes");
				help.showSingleFAQ("8", null);
			} else if (index == 3) {
				Dictionary<string, string> configMap = new Dictionary<string, string>();
				configMap.Add("gotoConversationAfterContactUs", "yes");
				help.showFAQSection("5", null);
			} else if (index == 4) {
				help.leaveBreadCrumb("this is a bread crumb");
			}
		}

	}

	// Use this for initialization
	void Start () {
		this.help = new Support();
		Dictionary<string, string> configMap = new Dictionary<string, string>();
		configMap.Add("disableInAppNotif", "no");
		configMap.Add("unityGameObject", "Cube");
		help.install("518c9d6472a68a2ab99a42c26567e24f", "test.helpshift.com", "test_platform_20121128080651806-01885df6cbd4394", configMap);

	}
	
	// Update is called once per frame
	void Update () {
			if(Input.GetMouseButtonDown(0)) {
			Debug.Log("Showing the support screen");
			ShowSupport();
		}
	}
}
