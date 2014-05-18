from django.conf.urls import patterns, include, url

from django.contrib import admin
admin.autodiscover()

urlpatterns = patterns('',
    # Examples:
    # url(r'^$', 'cetysclassroom.views.home', name='home'),
    # url(r'^blog/', include('blog.urls')),
	url(r'^$','classrooms.views.v_index'),
    url(r'^Classroom/(?P<Classroom>\w+( \w+)*)?\+?(?P<RP>\w+)?\+?(?P<X>\w+)?\+?(?P<Y>\w+)?$','classrooms.views.v_classroom'),
	url(r'^(?P<numero>\w+( \w+)*)?$','classrooms.views.v_search'),
	url(r'^(?P<salon>\w+( \w+)*)?\+?(?P<RP>\w+)?\+?(?P<X>\w+)?\+?(?P<Y>\w+)?$','classrooms.views.v_index2'),
    url(r'^admin/', include(admin.site.urls)),
)
