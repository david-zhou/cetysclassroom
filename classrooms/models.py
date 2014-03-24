from django.db import models

class Classroom(models.Model):
	Classroom_Name = models.CharField(max_length=30)
	Classroom_Image = models.CharField(max_length=10)
	Classroom_CoordX = models.IntegerField()
	Classroom_CoordY = models.IntegerField()

class Tags(models.Model):
	Tag_Name = models.CharField(max_length=20)

class Classroom_Tags(models.Model):
	Tag_ID = models.ForeignKey(Tags)
	Classroom_ID = models.ForeignKey(Classroom)

