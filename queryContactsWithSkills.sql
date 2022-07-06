
SELECT * FROM Contacts left join ContactSkill
	on Contacts.contactId=ContactSkill.ContactsId
left join Skills on ContactSkill.SkillsId=Skills.SkillId