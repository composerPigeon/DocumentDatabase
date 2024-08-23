#!/usr/bin/env python

import requests
import argparse
import json

def extract_message_from_json_result(response: requests.Response) -> str | None:

    data = response.content
    loaded_data: dict = json.loads(data)
    if response.ok:
        return loaded_data["ok"]
    else:
        return loaded_data["error"]

def execute_command(command: str, server_url: str) -> str:
    data: dict = {"command": command}

    response: requests.Response = requests.post(
        url=server_url,
        json=data
    )

    message: str = extract_message_from_json_result(response)

    if response.ok:
        return f"ok> {message}"
    else:
        return f"error> {message}"



def process_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser()

    parser.add_argument("url", help="url of the database server", type=str)

    return parser.parse_args()
    

if __name__ == "__main__":

    args: argparse.Namespace = process_args()

    while True:
        command: str = input("> ")
        print(execute_command(command, args.url))