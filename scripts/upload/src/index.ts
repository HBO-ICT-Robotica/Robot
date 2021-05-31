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
	host: "keyslam.nl",
	port: 8192,
	username: "ballboi",
	password: "winnaars",
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

			await sftp.put(filestream, "Robot" + file.relPath + file.name);
			console.log("Uploaded: " + file.relPath + file.name);
		};

		var ssh = new SSH2Promise({
			host: 'keyslam.nl',
			username: 'ballboi',
			password: 'winnaars',
			port: 8192,
		});
	
		await ssh.connect();
	
		var socket = await ssh.spawn("whoami");
		socket.on('data', (data: Buffer) => {
			console.log(data.toString());
		});
	})
	.catch((reason) => {
		console.log(reason);
	})
	.finally(() => {
		sftp.end();
	});