from django.shortcuts import render
from django.http import HttpResponseRedirect
from django.shortcuts import render_to_response
from django.template import RequestContext
# Create your views here.
def v_index(request):
	return render_to_response("index.html", context_instance = RequestContext(request))

def v_classroom(request,Classroom):
	return render_to_response("room.html", {"roomNumber":Classroom}, context_instance = RequestContext(request))