from django.db import models

class Salon(models.Model):
	Name = models.CharField(max_length=30)
	Image = models.CharField(max_length=10)
	
# Create your models here.
