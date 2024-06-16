'use client';

import { Modal, Textarea, Button, Center, TextInput, Text, Space, Stack } from '@mantine/core';
import { IFragment, IUpdateFragmentRequest } from '@/app/types';
import { useForm } from '@mantine/form';
import ModalTitle from '@/app/components/ModalTitle';
import updateFragment from '@/app/api/updateFragment';
import { useDisclosure } from '@mantine/hooks';
import markFragmentEntrypoint from '@/app/api/markFragmentEntrypoint';
import AddFragmentModal from './AddFragmentModal';
import { RichTextEditor, Link } from '@mantine/tiptap';
import { useEditor } from '@tiptap/react';
import Highlight from '@tiptap/extension-highlight';
import StarterKit from '@tiptap/starter-kit';
import Underline from '@tiptap/extension-underline';
import TextAlign from '@tiptap/extension-text-align';
import Superscript from '@tiptap/extension-superscript';
import SubScript from '@tiptap/extension-subscript';
import beautify from 'js-beautify';
import Files from '../../../components/Files';

interface IProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    fragment: IFragment;
    storyInfoId: string;
};

interface IUpdateProps {
    opened: boolean;
    close: () => void;
    storyInfoVersionId: string;
    onStoryChanged: () => void;
    fragment: IFragment;
    editorType: EditorType;
    storyInfoId: string;
}

enum EditorType {
    Rich,
    HTML
}

const UpdateModal = ({ opened, close, storyInfoVersionId, onStoryChanged, fragment, editorType, storyInfoId }: IUpdateProps) => {

    const beautifiedHtml = beautify.html(fragment.body, {
        indent_size: 2,
        wrap_line_length: 80
    });
    
    const form = useForm<IUpdateFragmentRequest>({
        initialValues: {
            name: fragment.name,
            storyInfoVersionId,
            body: beautifiedHtml,
            fragmentId: fragment.id,
        },
    });

    const editor = useEditor({
        extensions: [
            StarterKit,
            Underline,
            Link,
            Superscript,
            SubScript,
            Highlight,
            TextAlign.configure({ types: ['heading', 'paragraph'] }),
        ],
        content: fragment.body,
    });
    
    const onClick = async () => {
        if (editorType === EditorType.HTML) {
            await updateFragment(form.values);
        } else {
            const request = form.values;
            request.body = editor!.getHTML();
            await updateFragment(request);
        }

        onStoryChanged();
        close();
    };
    
    return (
      <Modal
          opened={opened}
          onClose={close}
          title={<ModalTitle>Обновление фрагмента</ModalTitle>}
          size={'xl'}>

          <label>Название фрагмента</label>
          <TextInput
              placeholder="Введите название фрагмента"
              mt="sm"
              {...form.getInputProps('name')}
          />
          
          <Space h={'md'} />
          {  editorType == EditorType.HTML ? (
              <>
                  <Textarea
                      label="HTML фрагмента"
                      placeholder="Введите HTML фрагмента"
                      autosize
                      minRows={3}
                      mt="sm"
                      {...form.getInputProps('body')}
                  />
                  <Space h={'md'} />
                  <div
                      className="html-preview"
                      dangerouslySetInnerHTML={{ __html: form.values.body }}
                      style={{ border: '1px solid #e0e0e0', padding: '10px', marginTop: '10px' }}
                  />

                  <Files storyInfoId={storyInfoId}/>
              </>
          ) : (
              <>
                  <label>Шаблон фрагмента</label>
                  
                  <RichTextEditor editor={editor}>
                      <RichTextEditor.Toolbar sticky stickyOffset={60}>
                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.Bold />
                              <RichTextEditor.Italic />
                              <RichTextEditor.Underline />
                              <RichTextEditor.Strikethrough />
                              <RichTextEditor.ClearFormatting />
                              <RichTextEditor.Highlight />
                              <RichTextEditor.Code />
                          </RichTextEditor.ControlsGroup>

                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.H1 />
                              <RichTextEditor.H2 />
                              <RichTextEditor.H3 />
                              <RichTextEditor.H4 />
                          </RichTextEditor.ControlsGroup>

                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.Blockquote />
                              <RichTextEditor.Hr />
                              <RichTextEditor.BulletList />
                              <RichTextEditor.OrderedList />
                              <RichTextEditor.Subscript />
                              <RichTextEditor.Superscript />
                          </RichTextEditor.ControlsGroup>

                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.Link />
                              <RichTextEditor.Unlink />
                          </RichTextEditor.ControlsGroup>

                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.AlignLeft />
                              <RichTextEditor.AlignCenter />
                              <RichTextEditor.AlignJustify />
                              <RichTextEditor.AlignRight />
                          </RichTextEditor.ControlsGroup>

                          <RichTextEditor.ControlsGroup>
                              <RichTextEditor.Undo />
                              <RichTextEditor.Redo />
                          </RichTextEditor.ControlsGroup>
                      </RichTextEditor.Toolbar>

                      <RichTextEditor.Content />
                  </RichTextEditor>
              </>
          )}
          
          <Space h={'md'} />
          <Center>
              <Button onClick={onClick}>
                  Обновить
              </Button>
          </Center>
      </Modal>  
    );
}

const UpdateFragmentModal = ({ opened, close, storyInfoVersionId, onStoryChanged, fragment, storyInfoId }: IProps) => {
    const [updateFragmentModalOpenedViaHtml, { open: openUpdateFragmentModalViaHtml, close: closeUpdateFragmentModalViaHtml }] = useDisclosure();
    const [updateFragmentModalOpenedViaEditor, { open: openUpdateFragmentModalViaEditor, close: closeUpdateFragmentModalViaEditor }] = useDisclosure();
    const [addFragmentModalOpened, { open: openAddFragmentModal, close: closeAddFragmentModal }] = useDisclosure();
    
    const onMarkFragmentEntrypoint = async () => {
        await markFragmentEntrypoint(fragment.id, storyInfoVersionId);
        onStoryChanged();
    };
    
    return (
        <Modal
            opened={opened}
            onClose={close}
            title={<ModalTitle>Редактирование фрагмента</ModalTitle>}
        >
            <UpdateModal 
                opened={updateFragmentModalOpenedViaHtml}
                close={closeUpdateFragmentModalViaHtml}
                storyInfoVersionId={storyInfoVersionId}
                onStoryChanged={onStoryChanged}
                fragment={fragment}
                editorType={EditorType.HTML}
                storyInfoId={storyInfoId}
            />

            <UpdateModal
                opened={updateFragmentModalOpenedViaEditor}
                close={closeUpdateFragmentModalViaEditor}
                storyInfoVersionId={storyInfoVersionId}
                onStoryChanged={onStoryChanged}
                fragment={fragment}
                editorType={EditorType.Rich}
                storyInfoId={storyInfoId}
            />
            
            <AddFragmentModal
                opened={addFragmentModalOpened}
                close={closeAddFragmentModal}
                fromFragmentId={fragment.id}
                storyInfoVersionId={storyInfoVersionId}
                onStoryChanged={onStoryChanged}
            />
            
            <Stack>
                <Button mr={'10px'} onClick={openUpdateFragmentModalViaHtml}>HTML</Button>
                <Button mr={'10px'} onClick={openUpdateFragmentModalViaEditor}>Редактор</Button>
                <Button mr={'10px'} onClick={openAddFragmentModal}>Добавить переход</Button>
                <Button onClick={onMarkFragmentEntrypoint}>Сделать входной точкой</Button>
            </Stack>
        </Modal>
    );
};

export default UpdateFragmentModal;