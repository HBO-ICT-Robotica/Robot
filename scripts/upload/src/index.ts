#!/usr/bin/env node

import client from "ssh2-sftp-client";
import SSH2Promise = require('ssh2-promise');
import fs, { ReadStream } from "fs";

type file = {
	name: string,
	relPath: string,
}

const blacklist = new Set<string>([
	".git",
	".gitignore",
	".vscode",
	"bin",
	"obj",
	"scripts",
	"LICENSE",
	"omnisharp.json",
	"README.md",
]);

const keypress = async () => {
  process.stdin.setRawMode(true)
  return new Promise<void>(resolve => process.stdin.once('data', () => {
    process.stdin.setRawMode(false)
    resolve()
  }))
}

const getAllFiles = function (rootPath: string, relPath: string, arrayOfFiles?: file[]): file[] {
	const files = fs.readdirSync(rootPath + relPath);

	arrayOfFiles = arrayOfFiles || []

	files.forEach(function (file) {
		if (blacklist.has(file)) {
			return;
		}

		if (fs.statSync(rootPath + relPath + file).isDirectory()) {
			arrayOfFiles = getAllFiles(rootPath, relPath + file + "/", arrayOfFiles)
		} else {
			arrayOfFiles.push({
				name: file,
				relPath: relPath,
			})
		}
	})

	return arrayOfFiles
}

const sftp = new client();

sftp.connect({
	host: "192.168.137.128",
	port: 22,
	username: "pi",
	password: "raspberry",
})
	.then(async () => {
		const path = process.argv[1].substr(0, process.argv[1].length - 20);

		const files = getAllFiles(path, "/");

		for (const file of files) {
			if (! await sftp.exists("Robot" + file.relPath)) {
				console.log("Creating folder");
				await sftp.mkdir("./Robot" + file.relPath, true);
			}

			const filestream = fs.createReadStream(path + file.relPath + file.name);

			console.log("Uploading: " + file.relPath + file.name);
			await sftp.put(filestream, "Robot" + file.relPath + file.name);
			console.log("Uploaded: " + file.relPath + file.name);
		};

		var ssh = new SSH2Promise({
			host: '192.168.137.128',
			username: 'pi',
			password: 'raspberry',
			port: 22,
		});
	
		console.log("Connecting");
		await ssh.connect();
		console.log("Connected");
		
		console.log("Starting command");
		ssh.spawn("sh runRobot.sh", [], { pty: true }).then(async (socket) => {
			console.log("Started command");
			socket.on('data', (data: Buffer) => {
				console.log(data.toString());
			})

			await keypress();
			console.log("Exiting");

			socket.close();
			process.exit(0);
		}).catch((reason) => {
			console.log(reason);
		});
	})
	.catch((reason) => {
		console.log(reason);
	})
	.finally(() => {
		sftp.end();
	});