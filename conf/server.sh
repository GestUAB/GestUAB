#!/bin/sh

### BEGIN INIT INFO
# Provides:          monoserve.sh
# Required-Start:    $local_fs $syslog $remote_fs
# Required-Stop:     $local_fs $syslog $remote_fs
# Default-Start:     2 3 4 5
# Default-Stop:      0 1 6
# Short-Description: Start fastcgi mono server with hosts
### END INIT INFO

#source /etc/mono-addon-env
NAME=monoserver
DESC=monoserver

#export MONO_LOG_LEVEL=debug
#export MONO_ENV_OPTIONS=--debug
MONOSERVER=$(which fastcgi-mono-server4)
MONOSERVER_PID=$(ps auxf | grep fastcgi-mono-server4.exe | grep -v grep | awk '{print $2}')

WEBAPPS="www.gestuab.com:/:/srv/www/htdocs/gestuab.com/"

case "$1" in
        start)
                if [ -z "${MONOSERVER_PID}" ]; then
                        if [ "$2" ] && [ "$2" == "debug" ]; then        
                                export MONO_OPTIONS="--debug"
                                echo "starting mono server in debug mode"
                                ${MONOSERVER} /applications=www.gestuab.com:/:/srv/www/htdocs/gestuab.com/ /socket=tcp:127.0.0.1:8888 /printlog=True /verbose=True /loglevels="All" &
                        else
                                echo "starting mono server"
                                ${MONOSERVER} /applications=www.gestuab.com:/:/srv/www/htdocs/gestuab.com/ /socket=tcp:127.0.0.1:8888 &
                        fi
                        echo "mono server started"
                else
                        echo ${WEBAPPS}
                        echo "mono server is running"
                fi
        ;;
        stop)
                if [ -n "${MONOSERVER_PID}" ]; then
                        kill ${MONOSERVER_PID}
                        echo "mono server stopped"
                else
                        echo "mono server is not running"
                fi
        ;;
esac

exit 0

