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
        // Load experience pool from save data
        int version = pkg.ReadInt();
        if (version >= 1) {
            _experience = pkg.ReadDouble();
        }
    }

    public void Save(ZPackage pkg)
    {
        // Version number, for backwards compatibility
        pkg.Write(1);
        pkg.Write(_experience);
    }

    public Player GetPlayer()
    {
        return GetComponent<Player>();
    }
}
