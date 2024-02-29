pushd "%~dp0"
set tmpath=%SCE_ROOT_DIR%\Prospero\Tools\Target Manager Server\bin
"%tmpath%\prospero-ctrl.exe" process kill 2>NUL
"%tmpath%\prospero-run.exe" /log:"eboot.log" /elf "Build\eboot.self"
popd
