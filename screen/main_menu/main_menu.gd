extends Control

@onready var first_button: Button = $VBoxContainer/StartButton
@onready var options_button: Button = $VBoxContainer/OptionsButton
@onready var options_menu: CanvasLayer = $"..".get_node("OptionsMenu")

func _ready() -> void:
	get_tree().paused = true
	first_button.grab_focus()

func _on_start_button_pressed() -> void:
	get_tree().paused = false
	get_tree().change_scene_to_file("res://level/cave/world.tscn")

func _on_options_button_pressed() -> void:
	options_menu.visible = true

func _on_exit_button_pressed() -> void:
	get_tree().quit()

func _on_options_menu_visibility_changed() -> void:
	visible = !options_menu.visible
	if visible:
		options_button.grab_focus()
