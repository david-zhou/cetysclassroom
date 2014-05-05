from django.conf.urls import patterns, include, url

from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'cetysclassroom.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),
	url(r'^$','classrooms.views.v_index'),
	url(r'^(?P<Classroom>\w+)?$','classrooms.views.v_classroom'),
    url(r'^admin/', include(admin.site.urls)),
)
