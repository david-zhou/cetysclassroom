from django.db import models

class Classroom(models.Model):
	Classroom_Name = models.CharField(max_length=30)
	Classroom_Image = models.CharField(max_length=10)
	Classroom_CoordX = models.IntegerField()
	Classroom_CoordY = models.IntegerField()
	Classroom_ReferencePoint = models.IntegerField()

# 1 = estacionamiento gym
# 2 = estacionamiento estadio
# 3 = aulas moviles
# 4 = biblioteca
# 5 = salas
# 6 = estacionamiento case
# 7 = estacionamiento auditorio
# 8 = licenciatura
# 9 = CAT
# 10 = centro de idiomas
# 11 = estacionamiento centro de idiomas

class Tags(models.Model):
	Tag_Name = models.CharField(max_length=20)

class Classroom_Tags(models.Model):
	Tag_ID = models.ForeignKey(Tags)
	Classroom_ID = models.ForeignKey(Classroom)

