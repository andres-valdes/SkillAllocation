using System;
using UnityEngine;

namespace Ratzu.SkillAllocation;

public class ExperiencePool : MonoBehaviour
{
    private double _experience;

    public double GetExperience()
    {
        return _experience;
    }

    public void AddExperience(double experience)
    {
        _experience += experience;
    }

    public void SubtractExperience(double experience) {
        _experience -= experience;
    }

    public void Load(ZPackage pkg)
    {
       // implement saving
    }

    public void Save(ZPackage pkg)
    {
        // implement saving
    }

    public Player GetPlayer()
    {
        return GetComponent<Player>();
    }
}
