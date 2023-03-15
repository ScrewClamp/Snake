using System;
using UnityEngine;

public class PolicyLink : MonoBehaviour
{
	public string policyPageURL = "https://armnomads.com/privacy_policy/linetracer_privacy_policy.html";

	public void OpenLink()
	{
		Application.OpenURL(this.policyPageURL);
	}
}
