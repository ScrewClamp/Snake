using System;
using UnityEngine;

public class BoosterCollector : MonoBehaviour
{
	public Sprite armorSprite;

	public SoundManager soundManager;

	public int armorHp;

	[Space(10f)]
	public Sprite autoBreakerSprite;

	public float autoBreakerDuration;

	public float autoBreakerSpeedIncreaseOffset;

	private void OnTriggerEnter2D(Collider2D other)
	{
		bool hasAnotherPowerUp = base.GetComponent<PowerUp>() != null;
		if (other.CompareTag("Armor"))
		{
			ArmorComponent armorComponent = base.gameObject.AddComponent<ArmorComponent>();
			armorComponent.armorHp = this.armorHp;
			armorComponent.armorSprite = this.armorSprite;
			armorComponent.soundManager = this.soundManager;
			this.RemoveOldBooster(hasAnotherPowerUp);
		}
		else if (other.CompareTag("AutoBreaker"))
		{
			AutoBreakerComponent autoBreakerComponent = base.gameObject.AddComponent<AutoBreakerComponent>();
			autoBreakerComponent.duration = this.autoBreakerDuration;
			autoBreakerComponent.autoBreakerSprite = this.autoBreakerSprite;
			autoBreakerComponent.speedIncreaseOffset = this.autoBreakerSpeedIncreaseOffset;
			autoBreakerComponent.soundManager = this.soundManager;
			this.RemoveOldBooster(hasAnotherPowerUp);
		}
	}

	private void RemoveOldBooster(bool hasAnotherPowerUp)
	{
		if (hasAnotherPowerUp)
		{
			PowerUp component = base.GetComponent<PowerUp>();
			component.Finish();
		}
	}
}
