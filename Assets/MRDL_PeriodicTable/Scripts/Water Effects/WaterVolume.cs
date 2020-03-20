using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class WaterVolume : MonoBehaviour {

	public GameObject particlePrefab;
	public float waterHeight=6.5f;
	public float emissionRate=50;
	public class inWaterObject
	{
		public Transform tar;
		float yLimit;
		float eRate;
		ParticleSystem ps;
		Transform tps;
		Vector3 lastPos;
		Vector3 velocity()
		{
			return new Vector3(tar.position.x-lastPos.x,0,tar.position.z-lastPos.z);
		}
		public inWaterObject(Transform _t,float _y,GameObject prefab,float _eRate)
		{
			yLimit = _y;
			eRate = _eRate;
			tar = _t;
			lastPos = _t.position; 
			ps = Instantiate(prefab,lastPos,_t.rotation).GetComponent<ParticleSystem>();
			tps = ps.transform;
		}
		public void UpdateFoam()
		{
			Vector3 vel = velocity();
			ParticleSystem.EmissionModule emissionModule = ps.emission;
			emissionModule.rateOverTime =eRate*vel.magnitude;
			tps.position = new Vector3(lastPos.x,yLimit,lastPos.z);
			if(vel!=Vector3.zero){
				tps.rotation = Quaternion.LookRotation(vel);
				var main = ps.main;
				main.startRotation = tps.rotation.eulerAngles.y* Mathf.Deg2Rad;
			}
			lastPos = tar.position;
		}
		public ParticleSystem Disable()
		{
			ParticleSystem.EmissionModule emissionModule = ps.emission;
			emissionModule.rateOverTime = 0;
			return ps;
		}
	}
	List<float> endTimes = new List<float>();
	List<ParticleSystem> toDestroy = new List<ParticleSystem>();
	List<inWaterObject> inside = new List<inWaterObject>();
	bool contained(Transform _t)
	{
		for(int i=0;i<inside.Count;i++)
		{
			if(inside[i].tar==_t)
			{
				return true;
			}
		}
		return false;
	}
	int containedInt(Transform _t)
	{
		for(int i=0;i<inside.Count;i++)
		{
			if(inside[i].tar==_t)
			{
				return i;
			}
		}
		return -1;
	}
	void OnCollisionEnter(Collision col)
	{
		if(!contained(col.transform))
		{
			inside.Add(new inWaterObject(col.transform,transform.position.y+waterHeight,particlePrefab,emissionRate));
		}
	}
	void OnTriggerExit(Collider col)
	{
		int i = containedInt(col.transform);
		if(i!=-1)
		{
			endTimes.Add(Time.time+3);
			toDestroy.Add(inside[i].Disable());
			inside.RemoveAt(i);
		}
	}
	void LateUpdate()
	{
		for(int i=inside.Count-1;i>=0;i--)
		{
			if(inside[i].tar==null)
			{
				inside.RemoveAt(i);
			}else{
				inside[i].UpdateFoam();
			}
		}
		for(int i=toDestroy.Count-1;i>=0;i--)
		{
			if(endTimes[i]<Time.time)
			{
				Destroy(toDestroy[i].gameObject);
				endTimes.RemoveAt(i);
				toDestroy.RemoveAt(i);
			}
		}
	}
}