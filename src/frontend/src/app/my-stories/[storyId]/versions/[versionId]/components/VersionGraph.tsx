'use client';

import { IStoryVersion, IBranch, IFragment } from '@/app/types';
import { Center } from '@mantine/core';
import React, { useState, useMemo, useEffect } from 'react';
import CytoscapeComponent from 'react-cytoscapejs';
import { useDisclosure } from "@mantine/hooks";
import UpdateBranchModal from './UpdateBranchModal';
import UpdateFragmentModal from './UpdateFragmentModal';

interface IProps {
    storyVersion: IStoryVersion;
    onStoryChanged: () => void;
}

const VersionGraph = ({ storyVersion, onStoryChanged }: IProps) => {
    const [updateBranchModalOpened, { open: openUpdateBranchModal, close: closeUpdateBranchModal }] = useDisclosure();
    const [updateFragmentModalOpened, { open: openUpdateFragmentModal, close: closeUpdateFragmentModal }] = useDisclosure();
    const [selectedBranch, setSelectedBranch] = useState<IBranch | null>(null);
    const [selectedFragment, setSelectedFragment] = useState<IFragment | null>(null);
    const branches = useMemo(() => storyVersion.components.flatMap(comp => comp.branches), [storyVersion.components]);
    const fragments = useMemo(() => storyVersion.components.flatMap(comp => comp.fragments), [storyVersion.components]);
    const elements = [
        ...fragments.map(fragment => ({
            data: { id: fragment.id, label: fragment.name }
        })),

        ...branches.map(branch => ({
            data: {
                id: branch.id,
                source: branch.fromFragmentId,
                target: branch.toFragmentId,
                label: branch.inscription
            }
        }))
    ];

    const style = [
        {
            selector: 'node',
            style: {
                'shape': 'round-rectangle',
                'label': 'data(label)',
                'text-valign': 'center',
                'text-halign': 'center',
                'background-color': '#f0f8ff',
                'border-color': '#0084ff',
                'border-width': 2,
                'width': 120,
                'height': 120
            }
        },
        {
            selector: 'edge',
            style: {
                'curve-style': 'bezier',
                'target-arrow-shape': 'triangle',
                'label': 'data(label)',
                'text-margin-y': '-10px',
                'font-size': '12px',
                'text-background-opacity': 1,
                'text-background-color': '#ffffff',
                'text-background-padding': '3px',
                'text-background-shape': 'rectangle',
            }
        },
    ];

    const handleFragmentClick = (event: any) => {
        const fragmentId = event.target.data().id;
        const fragment = fragments.find(f => f.id === fragmentId)!;
        setSelectedFragment(fragment);
    };

    const handleBranchClick = (event: any) => {
        const branchId = event.target.data().id;
        const branch = branches.find(b => b.id === branchId)!;
        setSelectedBranch(branch);
    };

    const handleFragmentClose = () => {
        setSelectedFragment(null);
        closeUpdateFragmentModal();
    };

    const handleBranchClose = () => {
        setSelectedBranch(null);
        closeUpdateBranchModal();
    };
    useEffect(() => {
        if (selectedBranch != null && !updateBranchModalOpened) {
            openUpdateBranchModal();
        }
    }, [selectedBranch]);

    useEffect(() => {
        console.log("AAAA", selectedFragment, updateFragmentModalOpened)
        if (selectedFragment != null && !updateFragmentModalOpened) {
            openUpdateFragmentModal();
        }
    }, [selectedFragment]);
    
    return (
        <>
            <Center w={'1100px'} h={'850px'}>
                <CytoscapeComponent
                    elements={elements}
                    style={{ width: '1000px', height: '750px' }}
                    stylesheet={style}
                    layout={{ name: 'breadthfirst', directed: true }}
                    cy={(cy: any) => {
                        cy.on('tap', 'node', handleFragmentClick);
                        cy.on('tap', 'edge', handleBranchClick);
                    }}
                />
            </Center>

            {selectedBranch &&
                <UpdateBranchModal
                    opened={updateBranchModalOpened}
                    close={handleBranchClose}
                    storyInfoVersionId={storyVersion.id}
                    onStoryChanged={onStoryChanged}
                    branch={selectedBranch} />}
            
            {selectedFragment &&
                <UpdateFragmentModal 
                    opened={updateFragmentModalOpened}
                    close={handleFragmentClose}
                    fragment={selectedFragment}
                    onStoryChanged={onStoryChanged}
                    storyInfoVersionId={storyVersion.id} />}
        </>
    );
};

export default VersionGraph;