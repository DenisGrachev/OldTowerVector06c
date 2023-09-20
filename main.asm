	DEVICE ZXSPECTRUM48

	org 100h
begin:
	jp start
start:
	di	
	xor a :  out (10h),a;отключить квазидиск
	ld a,#c3      ; установить переход (код команды JMP) в
	ld (0000h),a  ; нулевой адрес и
	ld (0038h),a  ; адрес вызова прерывания.
	ld hl,main : ld (0001h),hl
	ld hl,ints : ld (0039h),hl		
	jp main
	
MUTE_MUSIC = 0
SPRITES_LIMIT_TO_COINS = 16	
	include "include\system.a80"
	include "include\tables.a80"
	include "include\intro.a80"
	include "include\input.a80"
	include "include\ints.a80"
	include "include\tiles.a80"
	include "include\coins.a80"
	include "include\doors.a80"
	include "include\sprites.a80"
	include "include\level.a80"
	include "include\camera.a80"	
	include "include\enemies.a80"
	include "include\hero.a80"
	include "include\guns.a80"
	include "include\fx.a80"
	include "music\player.a80"
	include "include\sound.a80"
	include "include\gsGameplay.a80"
	include "include\gsMenu.a80"
	include "include\fades.a80"	
	include "include\strings.a80"	
	include "include\stars.a80"
	include "include\outro.a80"	
	include "include\dzx0_CLASSIC_z80.asm"
	include "include\music.a80"



START_LEVEL = 1

GS_GAMEPLAY = 0
GS_WAIT_FIRE = 1
GS_NEXT_LEVEL = 2
GS_RESTART_LEVEL = 3
GS_MAIN_MENU = 4
GS_INIT_MAIN_MENU = 5
GS_OUTRO = 6
GS_INIT_OUTRO = 7

gameState:	db GS_INIT_MAIN_MENU
nextGameState: db GS_RESTART_LEVEL

;gameState:	db GS_INIT_OUTRO
;nextGameState: db GS_OUTRO


main:
	ld sp,100h	
;========================================================
	xor a : ld (cameraSpeed),a : ld (ss+1),a :  ld (pfCount),a : ld a,64 : ld (pfDelta),a	
	ld a,GS_INIT_MAIN_MENU : ld (gameState),a
;========================================================
	ei : halt		
	call vktStop
	halt

	call showIntro

	IF MUTE_MUSIC=0	
	xor a : call playMusic
	ENDIF

	call startGame

mainLoop:	
	dup 1
	halt
	edup			
	ld a,(gameState)
	cp GS_GAMEPLAY : jp z,doGameplay
	cp GS_WAIT_FIRE: jp z,waitFireButton
	cp GS_NEXT_LEVEL: jp z,nextLevel
	cp GS_RESTART_LEVEL: jp z,restartLevel
	cp GS_MAIN_MENU : jp z,doMainMenu
	cp GS_INIT_MAIN_MENU : jp z,initMainMenu
	cp GS_OUTRO : jp z,doOutro
	cp GS_INIT_OUTRO : jp z,initOutro

	jp mainLoop

	savebin "ot.rom",begin,$-begin

	        IF (_ERRORS = 0)                                 					
			SHELLEXEC "!run.bat"	
			ENDIF



