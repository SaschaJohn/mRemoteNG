﻿using mRemoteNG.Connection;
using mRemoteNG.Connection.Protocol;
using mRemoteNG.Container;
using mRemoteNG.Tree.Root;
using NUnit.Framework;


namespace mRemoteNGTests.IntegrationTests
{
    public class ConnectionInheritanceIntegrationTests
    {
        private RootNodeInfo _rootNode;

        [SetUp]
        public void Setup()
        {
            _rootNode = new RootNodeInfo(RootNodeType.Connection);
        }

        [TearDown]
        public void Teardown()
        {
            _rootNode = null;
        }

        [Test]
        public void ConnectionsInheritFromCorrectFolder()
        {
            /**
             * Root
             * --Folder1
             * ----Connection1
             * ----Folder2
             * ------Connection2
             * ----Connection3 (inherits username)
             */
            var folder1 = new ContainerInfo {Username = "folder1User"};
            var folder2 = new ContainerInfo {Username = "folder2User"};
            var connection1 = new ConnectionInfo();
            var connection2 = new ConnectionInfo();
            var connection3 = new ConnectionInfo {Inheritance = {Username = true}};
            _rootNode.Add(folder1);
            folder1.AddRange(new []{connection1, folder2, connection3});
            folder2.Add(connection2);
            Assert.That(connection3.Username, Is.EqualTo(folder1.Username));
        }

        [Test]
        public void ConnectionWillInheritAllFromFolder()
        {
            var folder = new ContainerInfo() {Protocol = ProtocolType.SSH2, Username = "folderUser", Domain = "CoolDomain"};
            var connection = new ConnectionInfo() {Inheritance = {EverythingInherited = true}};
            _rootNode.Add(folder);
            folder.Add(connection);
            Assert.That(new object[] { connection.Protocol, connection.Username, connection.Domain }, Is.EquivalentTo(new object[] {folder.Protocol, folder.Username, folder.Domain}));
        }
    }
}