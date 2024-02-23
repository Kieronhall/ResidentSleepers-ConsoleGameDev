pushd "%~dp0"
orbis-ctrl pkill
orbis-run  /fsroot . /console:process /noprogress /log:"eboot.log" /elf "Build\eboot.self"
popd
