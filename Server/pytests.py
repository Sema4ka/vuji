import pytest
import SocketServer as sockServer
import FastApiServer as fastApiServer


@pytest.mark.parametrize("command, args, answer",
                         [(sockServer.COMMAND_HAVE_INVITE_SERVER, ['1', '9123694'], "701S:1:9123694"),
                          (sockServer.COMMAND_OPEN_CONNECT_SERVER, ['Server open'], "600S:Server open"),
                          (sockServer.COMMAND_INVITE_SERVER, ['Server Invite'], "700S:Server Invite"),
                          ])
def test_socket_server_generate_message(command, args, answer):
    assert sockServer.generate_message(command, *args) == answer


@pytest.mark.parametrize("array, answer", [(["user", "info"], '["user", "info"]'),
                                           (["foo", "bar", 1], '["foo", "bar", 1]'),
                                           ([10, 10], '[10, 10]'),
                                           ([None, 99], '[null, 99]'),
                                           ])
def test_fast_api_server_array_to_json(array, answer):
    assert fastApiServer.array_to_json(array) == answer
