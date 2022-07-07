SELECT DISTINCT * FROM (
SELECT Contacts.*, Skills.* FROM Skills left join ContactSkill
	on Skills.SkillId=ContactSkill.SkillsId
left join Contacts on ContactSkill.ContactsId=Contacts.ContactId
UNION
SELECT Contacts.*, Skills.* FROM Contacts left join ContactSkill
	on Contacts.contactId=ContactSkill.ContactsId
left join Skills on ContactSkill.SkillsId=Skills.SkillId) S
ORDER BY contactId, SkillId