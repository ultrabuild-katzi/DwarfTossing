extends Control

@onready var resume_button: Button = $VBoxContainer/VBoxContainer/Resume
@onready var options_button: Button = $VBoxContainer/VBoxContainer/Options
@onready var options_menu: CanvasLayer = $"..".get_node("OptionsMenu")

@onready var is_ready = true

func _on_pause_menu_visibility_changed() -> void:
	if !is_ready:
		return
	if !get_parent().visible:
		Input.mouse_mode = Input.MOUSE_MODE_CAPTURED
	else:
		Input.mouse_mode = Input.MOUSE_MODE_VISIBLE

	get_tree().paused = get_parent().visible
	resume_button.grab_focus()

func _on_resume_pressed() -> void:
	$"..".visible = false

func _on_options_pressed() -> void:
	options_menu.visible = true
	
func _on_quit_pressed() -> void:
	get_tree().change_scene_to_file("res://screen/main_menu/main_menu.tscn")

func _unhandled_input(event: InputEvent) -> void:
	if !visible || !event.is_action_pressed("pause_game"):
		return
	_on_resume_pressed()
	get_viewport().set_input_as_handled()

func _on_options_menu_visibility_changed() -> void:
	visible = !options_menu.visible
	if visible:
		options_button.grab_focus()
