from django.shortcuts import render
from django.http import HttpResponseRedirect
from django.shortcuts import render_to_response
from django.template import RequestContext
from classrooms.models import Classroom
from classrooms.models import Tags
from classrooms.models import Classroom_Tags
from django.db.models import F
# Create your views here.
def v_index(request):
	return render_to_response("index.html", context_instance = RequestContext(request))

def v_index2(request, salon, RP, X, Y):
	return render_to_response("index.html", {"numero":salon, "RP":RP, "X":X, "Y":Y}, context_instance = RequestContext(request))

def v_classroom(request,Classroom):
	return render_to_response("room.html", {"roomNumber":Classroom}, context_instance = RequestContext(request))

def v_search(request,numero):
	salon = Classroom.objects.raw("select C.id, classroom_coordx as X, classroom_coordy as Y, classroom_name as NAME, classroom_ReferencePoint as RP from classrooms_classroom AS C where C.classroom_Name = '"+numero+"'")
	if len(list(salon)) == 0:
		salon = Classroom.objects.raw("Select C.id, classroom_coordx as X, classroom_coordy as Y, classroom_name as NAME, classroom_ReferencePoint as RP from classrooms_classroom AS C INNER JOIN classrooms_classroom_tags AS CT ON C.id = CT.Classroom_ID_id INNER JOIN classrooms_tags AS T ON CT.Tag_ID_id = T.id WHERE T.Tag_Name = '"+numero+"'")
		return render_to_response("index.html", {"salon":salon}, context_instance = RequestContext(request))
	else:
		return render_to_response("index.html", {"salon":salon}, context_instance = RequestContext(request))
