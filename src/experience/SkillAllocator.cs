using System;

namespace Ratzu.SkillAllocation;

public class SkillAllocator
{
    private ExperiencePool _experiencePool;

    public SkillAllocator(ExperiencePool experiencePool) => _experiencePool = experiencePool;

    public double GetRemainingCost(Skills.SkillType skillType)
    {
        Skills.Skill skill = _experiencePool.GetPlayer().m_skills.GetSkill(skillType);
        return Math.Ceiling(skill.GetNextLevelRequirement() - skill.m_accumulator);
    }

    public bool AllocateSkill(Skills.SkillType skillType) {
        Skills.Skill skill = _experiencePool.GetPlayer().m_skills.GetSkill(skillType);
        double currentExperience = _experiencePool.GetExperience();
        double skillCost = GetRemainingCost(skillType);
        if (currentExperience < skillCost) {
            return false;
        }
        _experiencePool.SubtractExperience(skillCost);
        skill.Raise((float)skillCost);
        return true;
    }
}
